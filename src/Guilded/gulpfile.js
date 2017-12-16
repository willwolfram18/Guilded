/// <binding BeforeBuild='styles' />
var gulp = require('gulp');
var cleanCss = require("gulp-clean-css");
var less = require("gulp-less");
var path = require("path");
var rename = require("gulp-rename");
var typescript = require("gulp-typescript");

gulp.task("styles", function () {
    var styleDirectory = "./wwwroot/css";

    return gulp.src([styleDirectory + "/**/*.less", "!" + styleDirectory + "/**/_*.less"])
        .pipe(less({
            paths: [path.join(__dirname, "wwwroot", "css")]
        }))
        .pipe(gulp.dest("./wwwroot/css"))
        .pipe(cleanCss({
            level: {
                1: {
                    specialComments: false,
                }
            }
        }))
        .pipe(rename({ suffix: ".min"}))
        .pipe(gulp.dest("./wwwroot/css"));
});

gulp.task("scripts", function() {
    var tsProject = typescript.createProject("tsconfig.json");
    var tsResult = gulp.src("wwwroot/**/*.ts")
        .pipe(tsProject());
    
    return tsResult.js.pipe(gulp.dest("wwwroot"));
});