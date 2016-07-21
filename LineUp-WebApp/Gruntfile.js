/// <binding AfterBuild='concat' />
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-contrib-watch");
    grunt.loadNpmTasks("grunt-contrib-cssmin");
    grunt.loadNpmTasks('grunt-contrib-concat');

    grunt.initConfig({
        cssmin: {
            dist: {
                files: {
                    'wwwroot/css/site.min.css': ['Styles/site.css', 'Styles/**/*.css']
                }
            }
        },
        uglify: {
            my_target: {
                files: {
                    'wwwroot/scripts/app.min.js': ['Scripts/app.js', 'Scripts/appSetup.js', 'Scripts/Factories/*.js', 'Scripts/Controllers/**/*.js', 'Scripts/Directives/*.js']
                }
            }
        },
        concat: {
            js: {
                src: ['Scripts/app.js', 'Scripts/appSetup.js', 'Scripts/Factories/*.js', 'Scripts/Controllers/**/*.js', 'Scripts/Directives/*.js'],
                dest: 'wwwroot/scripts/app.js',
                options: {
                    separator: ';'
                }
            },
        },
        watch: {
            scripts: {
                files: ['Scripts/app.js', 'Scripts/appSetup.js', 'Scripts/Factories/*.js', 'Scripts/Controllers/**/*.js'],
                tasks: ['uglify', 'concat']
            }
        }
    });

    grunt.registerTask('defaultCSS', ['cssmin', 'watch']);
    grunt.registerTask('defaultUglifiedJS', ['uglify', 'watch']);
    grunt.registerTask('defaultJS', ['concat', 'watch']);

};