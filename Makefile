.PHONY: run-local build

run-local:
	dotnet run --project AlgorithmUI/SimulatedAnnealingUI.csproj --pathbase=/Algorithm-Webapp

build:
	rm -rf build
	dotnet publish --configuration Release --output build