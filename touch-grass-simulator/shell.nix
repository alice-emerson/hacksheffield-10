{ pkgs ? import <nixpkgs> {} }:
	pkgs.mkShell {
		nativeBuildInputs = with pkgs.buildPackages; [
			dotnet-sdk_9
      		dotnetPackages.Nuget
			];
}
