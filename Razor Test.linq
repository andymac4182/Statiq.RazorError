<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Configuration.EnvironmentVariables</NuGetReference>
  <NuGetReference Prerelease="true">Statiq.App</NuGetReference>
  <NuGetReference Prerelease="true">Statiq.Markdown</NuGetReference>
  <NuGetReference Prerelease="true">Statiq.Razor</NuGetReference>
  <Namespace>Statiq.App</Namespace>
  <Namespace>Statiq.Common</Namespace>
  <Namespace>Statiq.Core</Namespace>
  <Namespace>Statiq.Markdown</Namespace>
  <Namespace>Statiq.Razor</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
  <CopyLocal>true</CopyLocal>
</Query>

public static async Task<int> Main()
{
	Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));

	return await Bootstrapper
	  .CreateDefault(new string[0] { })
	  .BuildPipeline(
		"Pages",
		builder => builder
			.WithInputReadFiles("**/*.md")
			//.WithProcessModules(new RenderMarkdown())
			.WithProcessModules(new RenderRazor().WithLayout((FilePath)"contentful/_Layout.cshtml"))
			.WithOutputWriteFiles(".html")) 
	  .RunAsync();
}