# Version control systems

To avoid any problems with automatically generated files, exclude some Visual Scripting files from your version control
solution.

To exclude files from version control, include a file or configure your settings to specify which files and folders to
exclude:

1. Create a new file at the root of your project directory.

   > [!TIP]
   > The root of your project directory is at the level above your **Assets** folder.

2. Name the file based on your chosen version control system:

    - **Git**: `.gitignore`. For more information,
      see [Git's documentation on gitignore](https://git-scm.com/docs/gitignore).

    - **Unity Collab**: `.collabignore`. For more information, see
      the [Unity User Manual](https://docs.unity3d.com/Manual/UnityCollaborateIgnoreFiles.html).

    - **Subversion**: Ignore the files from your `svn:ignore` property or runtime configuration options. For more
      information, see Subversion's documentation
      on [Ignoring Unversioned Items](https://svnbook.red-bean.com/en/1.7/svn.advanced.props.special.ignore.html).

3. Open the file in a text editor.

4. Add the appropriate files or file patterns to your ignore file or configuration. For an example and more information,
   see [Ignore file template](#ignore-file-template).

> [!NOTE]
> If you have an issue when you try to create a `.gitignore` file on Windows, refer to Microsoft's documentation
> on [how to create a .gitignore file from the command line](https://docs.microsoft.com/en-us/azure/devops/repos/git/ignore-files?view=azure-devops&tabs=command-line#create-a-gitignore).

## Ignore file template

The following template ignores all core Visual Scripting files, but preserves your project settings and variables. It
also includes the standard Unity ignore directives for files that you can exclude from version control. For more
information, see the `Unity.gitignore` file included
in [GitHub's gitignore template repository](https://github.com/github/gitignore/blob/master/Unity.gitignore).

Refer to the comments in the template for which lines to comment or remove.

```
    # Optionally exclude these transient (generated) files, 
    # because they can be easily re-generated by the package

    Assets/Unity.VisualScripting.Generated/VisualScripting.Flow/UnitOptions.db
    Assets/Unity.VisualScripting.Generated/VisualScripting.Flow/UnitOptions.db.meta
    Assets/Unity.VisualScripting.Generated/VisualScripting.Core/Property Providers
    Assets/Unity.VisualScripting.Generated/VisualScripting.Core/Property Providers.meta

    ## Unity
    # From: https://github.com/github/gitignore/blob/master/Unity.gitignore

    /[Ll]ibrary/
    /[Tt]emp/
    /[Oo]bj/
    /[Bb]uild/
    /[Bb]uilds/
    /[Ll]ogs/
    /[Uu]ser[Ss]ettings/

    # MemoryCaptures can get excessive in size.
    # They also could contain extremely sensitive data
    /[Mm]emoryCaptures/

    # Asset meta data should only be ignored when the corresponding asset is also ignored
    !/[Aa]ssets/**/*.meta

    # Uncomment this line if you want to ignore the asset store tools plugin
    # /[Aa]ssets/AssetStoreTools*

    # Autogenerated Jetbrains Rider plugin
    /[Aa]ssets/Plugins/Editor/JetBrains*

    # Visual Studio cache directory
    .vs/

    # Gradle cache directory
    .gradle/

    # Autogenerated VS/MD/Consulo solution and project files
    ExportedObj/
    .consulo/
    *.csproj
    *.unityproj
    *.sln
    *.suo
    *.tmp
    *.user
    *.userprefs
    *.pidb
    *.booproj
    *.svd
    *.pdb
    *.opendb
    *.VC.db

    # Unity3D generated meta files
    *.pidb.meta
    *.pdb.meta
    *.mdb.meta

    # Unity3D Generated File On Crash Reports
    sysinfo.txt

    # Builds
    *.apk
    *.aab
    *.unitypackage

    # Crashlytics generated file
    crashlytics-build.properties

    # Packed Addressables
    /[Aa]ssets/[Aa]ddressable[Aa]ssets[Dd]ata/*.*.bin*

    # Temporary auto-generated Android Assets
    /[Aa]ssets/[Ss]treamingAssets/aa.meta
    /[Aa]ssets/[Ss]treamingAssets/aa/*

```

## Remove previously committed files

If you committed any files to a version control solution that you want to exclude:

- See Git's documentation on [the `git-rm` command](https://git-scm.com/docs/git-rm).
- See Subversion's documentation
  on [the `svn delete` command](https://svnbook.red-bean.com/en/1.6/svn.ref.svn.c.delete.html).