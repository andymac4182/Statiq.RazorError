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
			.WithProcessModules(new RenderMarkdown())
			.WithOutputWriteFiles(".html")) 
	  .BuildPipeline(
	  	"Contentful",
		builder => builder
			.WithInputModules(new ContentfulDocumentModule())
			.WithProcessModules(new RenderRazor().WithLayout((FilePath)"contentful/_Layout.cshtml"))
			.WithOutputWriteFiles(".html")
	  )
	  .RunAsync();
}


namespace ContentfulContentTypes
{
	public class ContentPageDocument
	{
		//public SystemProperties Sys { get; set; }
		public string InternalName { get; set; }
		public string Slug { get; set; }
		public string PageTitle { get; set; }
		public string PageContent { get; set; } //
	}
}

public class ContentfulDocumentModule : Statiq.Common.Module
{
	protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
	{
		var result2 = new List<ContentfulContentTypes.ContentPageDocument>() {
			new ContentfulContentTypes.ContentPageDocument{ InternalName = "page 1", PageContent = "Test", PageTitle = "Page 1", Slug = "" },
			new ContentfulContentTypes.ContentPageDocument{ InternalName = "page 2", PageContent = "Test", PageTitle = "Page 2", Slug = "testing" },
		};

		var pages = result2.Take(2).Select(r => new { FileName = r.Slug, Items = new Dictionary<string, object> { { nameof(r.InternalName), r.InternalName }, { nameof(r.Slug), r.Slug }, { nameof(r.PageTitle), r.PageTitle }, { nameof(r.PageContent), r.PageContent } } });

		return pages.Select(p => context.CreateDocument(Path.Combine(p.FileName ?? "", "index.html"), p.Items.ToList())).ToList();
	}
}