## Build

First, put `Directory.Build.props` with the following content in the root directory of the project.
Change the `ElinGamePath` to the path of your Elin installation.

```xml
<Project>
  <PropertyGroup>
    <ElinGamePath>C:\Program Files (x86)\Steam\steamapps\common\Elin</ElinGamePath>
  </PropertyGroup>
</Project>
```

Then, run the following command to build the project.

```console
dotnet build
```
