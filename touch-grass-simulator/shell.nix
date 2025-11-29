{ pkgs ? import <nixpkgs> {} }:
	pkgs.mkShell {
		nativeBuildInputs = with pkgs.buildPackages; [
					dotnet-sdk_9
      		dotnetPackages.Nuget
					mono
			];

			LD_LIBRARY_PATH = with pkgs; lib.makeLibraryPath [
					freetype
					libGL
					pulseaudio
					xorg.libX11
					xorg.libXrandr
			];
}
