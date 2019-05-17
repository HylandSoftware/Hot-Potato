def host = '$WORKSPACE/src/HotPotato.AspNetCore.Host'
def middleware = '$WORKSPACE/src/HotPotato.AspNetCore.Middleware'
def core = '$WORKSPACE/src/HotPotato.Core'
def openapi = '$WORKSPACE/src/HotPotato.OpenApi'
def TestFeed = 'https://proget.onbase.net/nuget/TestFeed/'
def NuGetFeed = 'https://proget.onbase.net/nuget/NuGet/'

pipeline {
    agent {
        kubernetes {
            label 'hot-potato'
            yamlFile './build-spec.yml'
        }
    }

    stages {
        stage('Version') {
            environment {
                IGNORE_NORMALISATION_GIT_HEAD_MOVE = "1"
            }
            steps {
                container('gitversion') {
                    script {
                        env.IMAGE_VERSION = sh(script: 'mono /usr/lib/GitVersion/GitVersion.exe /output json /showvariable MajorMinorPatch', returnStdout: true).trim()
                        echo IMAGE_VERSION 
                    }
                }
            }
        }

        stage("Build") {
            steps {
                container("builder") {
                    sh 'dotnet build --configuration Release -p:Version=${IMAGE_VERSION}'
                }
            }
        }

        stage("Run-Unit-Tests") {
            steps {
                container("builder") {
                    // sh 'dotnet test ./test/HotPotato.Core.Test/*.csproj --configuration Release -r core-test-results.xml --no-restore --no-build'
                    // sh 'dotnet test ./test/HotPotato.AspNetCore.Middleware.Test/**/*.csproj --configuration Release -r middleware-test-results.xml --no-restore --no-build'
                    // sh 'dotnet test ./test/HotPotato.OpenApi.Test/**/*.csproj --configuration Release -r openapi-test-results.xml --no-restore --no-build'
                    sh 'dotnet test ./test/**/*.csproj'
                }
            }
        }
		
		stage("Run-Integration-Tests") {
            steps {
                container("builder") {
                    sh 'dotnet test ./test/HotPotato.Integration.Test/**/*.csproj --configuration Release -r integration-test-results.xml --no-restore --no-build'
                }
            }
        }

        stage("Run-E2E-Tests") {
            steps {
                container("builder") {
                    sh 'dotnet test ./test/HotPotato.E2E.Test/**/*.csproj --configuration Release -r E2E-test-results.xml --no-restore --no-build'
                }
            }
        }

        stage("Deploy") {
            when {
                branch 'feat/AUTOTEST-326-deploy-hotpotato-packages-to-nuget' //'master'
            }
            environment {
                API_KEY = credentials('testautomation_push')
            }
            steps {
                container("builder") {
                    sh 'dotnet pack ${host}/**/*.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ${middleware}/**/*.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ${core}/**/*.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ${openapi}/**/*.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'

                    //sh 'dotnet nuget push $WORKSPACE/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/TestFeed/' //https://proget.onbase.net/nuget/NuGet/
                    sh 'dotnet nuget push ${host}/**/*.nupkg -k ${API_KEY} -s ${TestFeed}' //https://proget.onbase.net/nuget/NuGet/
                    sh 'dotnet nuget push ${middleware}/**/*.nupkg -k ${API_KEY} -s ${TestFeed}'
                    sh 'dotnet nuget push ${core}/**/*.nupkg -k ${API_KEY} -s ${TestFeed}'
                    sh 'dotnet nuget push ${openapi}/**/*.nupkg -k ${API_KEY} -s ${TestFeed}'

                }
            }
        }
    }
    post {
        regression {
            mattermostSend color: "#ef1717", icon: "https://jenkins.io/images/logos/jenkins/jenkins.png", message: "Someone broke ${env.BRANCH_NAME}, Ref build number -- ${env.BUILD_NUMBER}! (<${env.BUILD_URL}|${env.BUILD_URL}>)"
        }

        fixed {
            mattermostSend color: "#7FFF00", icon: "https://jenkins.io/images/logos/jenkins/jenkins.png", message: "Someone fixed ${env.BRANCH_NAME}, Ref build number -- ${env.BUILD_NUMBER}! (<${env.BUILD_URL}|${env.BUILD_URL}>)"
        }
    }
}