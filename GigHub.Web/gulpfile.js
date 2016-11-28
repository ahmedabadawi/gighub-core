"use strict";

var gulp = require("gulp"),
    less = require("gulp-less"),
    path = require("path");

var paths = {
    webroot: "./wwwroot/"
};

paths.less = paths.webroot + "css/site.less";
paths.css = paths.webroot + "css/";
paths.minCss = paths.webroot + "css/site.min.css";

gulp.task('less', function() {
    return gulp.src(paths.less)
        .pipe(less({
            paths: [ path.join(__dirname, 'less', 'includes')]
        }))
        .pipe(gulp.dest(paths.css));
});
