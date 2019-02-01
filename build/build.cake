//Build script for Hot-Potato.NET
var HotPotatsln = "../HotPotato.sln";
var HotPotatoTest = "../test/HotPotato.Test/HotPotato.Test.csproj";

var target = Argument("target", "Default");

Task("NuGet-Restore")
	.Does(() => {
		DotNetCoreRestore(HotPotatsln);
	});

Task("Build")
	.IsDependentOn("NuGet-Restore")
	.Does(() => {
		DotNetCoreBuild(HotPotatsln, new DotNetCoreBuildSettings { NoRestore = true });
	});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() => {
		DotNetCoreTest(HotPotatoTest, new DotNetCoreTestSettings {
			VSTestReportPath = "results.xml",
			NoRestore = true,
			NoBuild = true
		});
	});

Task("Default")
	.IsDependentOn("Run-Unit-Tests");

RunTarget(target);
