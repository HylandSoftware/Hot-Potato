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
                    sh './build.sh -t NuGet-Restor'
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

        stage("Run-Unit-Tests") {
            steps {
                container("builder") {
                    sh './build.sh -t Run-Unit-Tests'
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

        stage("Run-E2E-Tests") {
            steps {
                container("builder") {
                    sh './build.sh -t Run-E2E-Tests'
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