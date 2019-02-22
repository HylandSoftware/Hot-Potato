pipeline {
    agent {
        kubernetes {
            label 'hot-potato'
            yamlFile './build-spec.yml'
        }
    }

    stages {
		stage("NuGet-Restore") {
            steps {
                container("builder") {
                    sh 'chmod +x ./build.sh'
                    sh './build.sh -t NuGet-Restore'
                }
            }
        }

        stage("Build") {
            steps {
                container("builder") {
                    sh './build.sh -t Build'
                }
            }
        }

        stage("Run-OpenApi-Tests") {
            steps {
                container("builder") {
                    sh './build.sh -t Run-OpenApi-Tests'
                }
            }
        }
		
		stage("Run-Integration-Tests") {
            steps {
                container("builder") {
                    sh './build.sh -t Run-Integration-Tests'
                }
            }
        }

    }
}