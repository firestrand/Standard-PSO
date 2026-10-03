#!/usr/bin/env python3
"""Compare the current implementation with a pinned, unmodified Git baseline."""
import io
import os
import sys
import json
from pathlib import Path
import subprocess
import tarfile
import tempfile

ROOT = Path(__file__).resolve().parent.parent
CONFIG = json.loads((ROOT / 'verification/config.json').read_text())
OUTPUT = Path(sys.argv[1]).resolve() if len(sys.argv) > 1 else ROOT / 'verification/results.json'

def run(args, cwd):
    subprocess.run(args, cwd=cwd, check=True, env={**os.environ, "DOTNET_TieredCompilation": "0"})

with tempfile.TemporaryDirectory(prefix='pso-ablation-') as folder:
    temp = Path(folder)
    old = temp / 'baseline'
    old.mkdir()
    archive = subprocess.check_output(['git', 'archive', CONFIG['baseline']], cwd=ROOT)
    with tarfile.open(fileobj=io.BytesIO(archive)) as tar:
        tar.extractall(old, filter='data')
    project = old / CONFIG['project']
    content = project.read_text(encoding='utf-8-sig')
    content = content.replace('</Project>', '<PropertyGroup><AssemblyName>BaselineAlgorithm</AssemblyName></PropertyGroup></Project>')
    project.write_text(content)
    run(['dotnet', 'build', str(project), '-c', 'Release', '--nologo'], old)
    baseline_dll = project.parent / 'bin/Release/net10.0/BaselineAlgorithm.dll'
    runner = temp / 'runner'
    runner.mkdir()
    (runner / 'Program.cs').write_text((ROOT / 'verification/Program.cs').read_text())
    for helper in CONFIG.get('helpers', []):
        (runner / helper).write_text((ROOT / 'verification' / helper).read_text())
    (runner / 'Runner.csproj').write_text(f'''<Project Sdk="Microsoft.NET.Sdk">
<PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework><EnableDefaultCompileItems>true</EnableDefaultCompileItems></PropertyGroup>
<ItemGroup><ProjectReference Include="{ROOT / CONFIG['project']}" />
<Reference Include="BaselineAlgorithm"><HintPath>{baseline_dll}</HintPath><Aliases>baseline</Aliases></Reference></ItemGroup>
</Project>''')
    run(['dotnet', 'run', '--project', str(runner), '-c', 'Release', '--', str(OUTPUT)], ROOT)
