/// <binding BeforeBuild='styles' />
var gulp = require('gulp');
var cleanCss = require("gulp-clean-css");
var less = require("gulp-less");
var path = require("path");
var rename = require("gulp-rename");

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