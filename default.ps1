
properties {
    $ouputDirectory = "output\"
    $msbuild = "C:\Program Files (x86)\MSBuild\14.0\bin\msbuild.exe"
    $locationOfSourcecode = "$relativePathToRoot\src"
    $nunitrunner = "$locationOfSourcecode\packages\NUnit.ConsoleRunner.*\tools\nunit*-console.exe" 
}

Task Clean {
    if(Test-Path $ouputDirectory){
        Remove-Item $ouputDirectory -Recurse -Force 
    }
    New-Item $ouputDirectory -type directory
}

Task Build {
	  Exec { & $msbuild @("$locationOfSourcecode\$solution", "/target:Build", "/p:Configuration=Release", "/p:OutputPath=$ouputDirectory", "/nologo", "/m", "/verbosity:quiet") }

}

Task Test {
    $tests = (Get-ChildItem $outputDirectory -Recurse -Include *Tests.dll)
    Assert (Test-Path $nunitrunner) "NUnit runner not found. Verify that NUnit runners are a dependency on your project."
    $nunitrunner = (Resolve-Path $nunitrunner)
    Exec { & $nunitrunner $tests "--agents=1" "--result=$testResultPath;format=nunit2" }
}


Task default -Depends Clean



