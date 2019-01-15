//Build script for Hot-Potato.NET
var target = Argument("target", "Default");

Task("NuGet-Restore")
	.Does(() => {
		NuGetRestore("../HotPotato.sln");
});

Task("Build")
	.IsDependentOn("NuGet-Restore")
	.Does(() => {
		DotNetBuild("../HotPotato.sln");
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() => {
		DotNetCoreTest("../test/HotPotato.Test/HotPotato.Test.csproj", new DotNetCoreTestSettings {VSTestReportPath = "results.xml"});
});

Task("Default")
	.IsDependentOn("Run-Unit-Tests");

RunTarget(target);
