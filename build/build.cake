//Build script for Hot-Potato.NET
var HotPotatsln = "../HotPotato.sln";
var HotPotatoTest = "../test/HotPotato.Test/HotPotato.Test.csproj";

var target = Argument("target", "Default");

Task("NuGet-Restore")
	.Does(() => {
		NuGetRestore(HotPotatsln);
	});

Task("Build")
	.IsDependentOn("NuGet-Restore")
	.Does(() => {
		DotNetBuild(HotPotatsln);
	});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() => {
		DotNetCoreTest(HotPotatoTest, new DotNetCoreTestSettings {VSTestReportPath = "results.xml"});
	});

Task("Default")
	.IsDependentOn("Run-Unit-Tests");

RunTarget(target);
