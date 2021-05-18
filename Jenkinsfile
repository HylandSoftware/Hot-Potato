pipeline {
    agent {
        kubernetes {
            label "hot-potato-${UUID.randomUUID().toString()}"
            yamlFile './build-spec.yml'
        }
    }
    environment {
        TEAMS_WEBHOOK_ID = credentials("teams-webhook-id")
    }

    stages {
        stage('Version') {
            environment {
                IGNORE_NORMALISATION_GIT_HEAD_MOVE = "1"
            }
            steps {
                container('gitversion') {
                    script {
                        env.IMAGE_VERSION = sh(script:'/tools/dotnet-gitversion . /output json /showvariable MajorMinorPatch', returnStdout: true).trim()
                        //keeping this comment in the case of needing a future alpha test
                        //IMAGE_VERSION = IMAGE_VERSION + "-alpha"
                        sh "echo $IMAGE_VERSION" 
                    }
                }
            }
        }

        stage("Build") {
            steps {
                container("builder31") {
                    sh 'dotnet build --configuration Release -p:Version=${IMAGE_VERSION}'
                }
            }
        }

        stage("Run-Unit-Tests") {
            steps {
                container("builder21") {
                    sh 'dotnet test ./test/HotPotato.Core.Test/HotPotato.Core.Test.csproj -f "netcoreapp2.2" -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/coreCoverage.xml -p:Exclude="[xunit.*]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/coreResults.xml" --no-restore --no-build'
                    sh 'dotnet test ./test/HotPotato.AspNetCore.Middleware.Test/HotPotato.AspNetCore.Middleware.Test.csproj -f "netcoreapp2.2" -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/middlewareCoverage.xml -p:Include="[*.Middleware]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/middlewareResults.xml" --no-restore --no-build'
                    sh 'dotnet test ./test/HotPotato.OpenApi.Test/HotPotato.OpenApi.Test.csproj -f "netcoreapp2.2" -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/openApiCoverage.xml -p:Include="[*.OpenApi]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/openapiResults.xml" --no-restore --no-build'
                }
                container("builder31") {
                    sh 'dotnet test ./test/HotPotato.Core.Test/HotPotato.Core.Test.csproj -f "netcoreapp3.1" -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/coreCoverage.xml -p:Exclude="[xunit.*]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/coreResults.xml" --no-restore --no-build'
                    sh 'dotnet test ./test/HotPotato.AspNetCore.Middleware.Test/HotPotato.AspNetCore.Middleware.Test.csproj -f "netcoreapp3.1" -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/middlewareCoverage.xml -p:Include="[*.Middleware]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/middlewareResults.xml" --no-restore --no-build'
                    sh 'dotnet test ./test/HotPotato.OpenApi.Test/HotPotato.OpenApi.Test.csproj -f "netcoreapp3.1" -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/openApiCoverage.xml -p:Include="[*.OpenApi]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/openapiResults.xml" --no-restore --no-build'
                }
            }
        }
		
		stage("Run-Integration-Tests") {
            steps {
                container("builder21") {
                    sh 'dotnet test ./test/HotPotato.Integration.Test/HotPotato.Integration.Test.csproj -f "netcoreapp2.2" -c Release -l:"JUnit;LogFilePath=$WORKSPACE/test/results/integrationResults.xml" --no-restore --no-build'
                }
                container("builder31") {
                    sh 'dotnet test ./test/HotPotato.Integration.Test/HotPotato.Integration.Test.csproj -f "netcoreapp3.1" -c Release -l:"JUnit;LogFilePath=$WORKSPACE/test/results/integrationResults.xml" --no-restore --no-build'
                }
            }
        }

        stage("Run-E2E-Tests") {
            steps {
                container("builder21") {
                    sh 'dotnet test ./test/HotPotato.E2E.Test/HotPotato.E2E.Test.csproj -f "netcoreapp2.2" -c Release -l:"JUnit;LogFilePath=$WORKSPACE/test/results/E2EResults.xml" --no-restore --no-build'
                }
                container("builder31") {
					sh 'dotnet test ./test/HotPotato.TestServer.Test/HotPotato.TestServ.Test.csproj -c Release -l:"JUnit;LogFilePath=$WORKSPACE/test/results/testServerResults.xml" --no-restore --no-build'
                    sh 'dotnet test ./test/HotPotato.E2E.Test/HotPotato.E2E.Test.csproj -f "netcoreapp3.1" -c Release -l:"JUnit;LogFilePath=$WORKSPACE/test/results/E2EResults.xml" --no-restore --no-build'
                    sh 'dotnet $WORKSPACE/src/HotPotato.AspNetCore.Host/bin/Release/netcoreapp3.1/HotPotato.AspNetCore.Host.dll &'
                    sh 'dotnet $WORKSPACE/test/HotPotato.Api/bin/Release/netcoreapp3.1/HotPotato.Api.dll &'
                }
                container("newman") {
                    sh 'newman run $WORKSPACE/test/HappyPathTests.postman_collection.json'
                    sh 'newman run $WORKSPACE/test/Non-ConformantTests.postman_collection.json'
                    sh 'newman run $WORKSPACE/test/NotInSpecTests.postman_collection.json'
                }
            }
        }

        stage("Deploy") {
            when {
                branch 'master'
            }
            environment {
                API_KEY = credentials('testautomation_push')
            }
            steps {
                container("builder31") {
                    sh 'dotnet pack ./src/HotPotato.AspNetCore.Host/HotPotato.AspNetCore.Host.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.AspNetCore.Middleware/HotPotato.AspNetCore.Middleware.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.Core/HotPotato.Core.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.OpenApi/HotPotato.OpenApi.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'

                    sh 'dotnet nuget push $WORKSPACE/src/HotPotato.AspNetCore.Host/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/NuGet/'
                    sh 'dotnet nuget push $WORKSPACE/src/HotPotato.AspNetCore.Middleware/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/NuGet/'
                    sh 'dotnet nuget push $WORKSPACE/src/HotPotato.Core/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/NuGet/'
                    sh 'dotnet nuget push $WORKSPACE/src/HotPotato.OpenApi/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/NuGet/'
                }
            }
        }
        stage("Push images") {
            when {
                branch 'master'
            }
            steps {
                container("docker") {
                    withDockerRegistry([credentialsId: 'hcr-tfsbuild', url: 'https://hcr.io']) {
                        sh 'docker build --tag hcr.io/automated-testing/hot-potato:${IMAGE_VERSION} --build-arg IMAGE_VERSION=${IMAGE_VERSION} .'
                        sh 'docker push hcr.io/automated-testing/hot-potato:${IMAGE_VERSION}'
                        sh 'docker tag hcr.io/automated-testing/hot-potato:${IMAGE_VERSION} hcr.io/automated-testing/hot-potato:latest'
                        sh 'docker push hcr.io/automated-testing/hot-potato:latest'
                    }
                }
            }
        }
    }
    post {
        always {
            junit '**/test/results/*.xml'
            cobertura coberturaReportFile: '**/test/coverage/*.xml'
            codometer programName: 'R&D Operations', projectName: 'HotPotato', teamName: 'TATO', channel: 'master', version: "${IMAGE_VERSION}", tags: [type: "Russet"]
        }
		regression {
            office365ConnectorSend webhookUrl: env.TEAMS_WEBHOOK_ID,
                    color: '#ff0000',
                    message: "${env.JOB_NAME} ${env.BUILD_NUMBER} failed to build.",
                    status: 'Failure'
        }
        fixed {
            office365ConnectorSend webhookUrl: env.TEAMS_WEBHOOK_ID,
                    color: '#00ff00',
                    message: "${env.JOB_NAME} ${env.BUILD_NUMBER} built successfully!",
                    status: 'Success'
        }
    }
}