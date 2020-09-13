.PHONY: run-local build run-build build-and-run

build:
	rm -rf build
	dotnet publish --configuration Release --output build

run-local:
	dotnet run --project AlgorithmUI/SimulatedAnnealingUI.csproj --pathbase=/Algorithm-Webapp

run-build:
	docker-compose up

build-and-run:
	rm -rf build
	dotnet publish --configuration Release --output build
	docker-compose up
