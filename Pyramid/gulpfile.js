/// <binding />
const gulp = require('gulp');
const gutil = require('gulp-util');
var babel = require('gulp-babel');
var minify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
const fs = require('fs');
const path = require('path');
const browserify = require('browserify');
const watchify = require('watchify');
const fsPath = require('fs-path');

var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');
var es2015 = require('babel-preset-es2015');




var less = require('gulp-less');
var concat = require('gulp-concat');
var rename = require('gulp-rename');
var gulpMinifyCss = require('gulp-minify-css');
var notify= require('gulp-notify');
var order = require("gulp-order");

function getFolders(dir) {
    return fs.readdirSync(dir)
    .filter(function (file) {
        return fs.statSync(path.join(dir, file)).isDirectory();
    });
}

const paths = [
    process.env.INIT_CWD + '\\ViewModels\\home',
    process.env.INIT_CWD + '\\ViewModels\\home\\components',
    process.env.INIT_CWD + '\\ViewModels\\common\\components'
];

function watchFolder(input, output) {
    var b = browserify({
        entries: [input],
        cache: {},
        packageCache: {},
        plugin: [watchify],
        basedir: process.env.INIT_CWD,
        paths: paths
    });

    function bundle() {
        b.bundle()
            .pipe(source('bundle.js'))
            .pipe(buffer())
            .pipe(sourcemaps.init({ loadMaps: true }))
            //.pipe(babel({ compact: false, presets: ['es2015'] }))
            // Add transformation tasks to the pipeline here.
            //.pipe(minify())
            //  .on('error', gutil.log)
            .pipe(sourcemaps.write('./'))
            .pipe(gulp.dest(output));

        gutil.log("Bundle rebuilt!");
    }
    b.on('update', bundle);
    bundle();
}

function compileJS(input, output) {
    // set up the browserify instance on a task basis
    var b = browserify({
        debug: true,
        entries: [input],
        basedir: process.env.INIT_CWD,
        paths: paths
    });

    return b.bundle()
    .pipe(source('bundle.js'))
    .pipe(buffer())
    .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(babel({ compact: false, presets: ['es2015'] }))
        // Add transformation tasks to the pipeline here.
        .pipe(minify())
        .on('error', gutil.log)
    //.pipe(sourcemaps.write('./'))
    .pipe(gulp.dest(output));
}

const scriptsPath = 'ViewModels';

gulp.task('build', function () {
    var folders = getFolders(scriptsPath);
    gutil.log(folders);
    folders.map(function (folder) {
        compileJS(scriptsPath + "//" + folder + "//main.js", "Scripts//app//" + folder);
    });
});

gulp.task('default', function () {
    var folders = getFolders(scriptsPath);
    gutil.log(folders);
    folders.map(function (folder) {
        watchFolder(scriptsPath + "//" + folder + "//main.js", "Scripts//app//" + folder);
    });

});


gulp.task('less', function () {
    return gulp.src('./Content/src/blocks/**/*.less')
        .pipe(order([
            "other.less",
            "*.less"
        ]))
        .pipe(less())
        .on("error", notify.onError(function (err) {
            return {
                title: "less-styles",
                message: err.message
            }
        }))
        .pipe(concat('main.css'))
        .pipe(gulp.dest('./Content/css'))
    ;
   
});


gulp.task('js', function () {
    return gulp.src('./Content/src/blocks/**/*.js')

        .pipe(concat('main.js'))
        .pipe(gulp.dest('./Scripts/main/'));
});


gulp.task('watch-js', function () {
    gulp.watch("./src/blocks/**/*.js", ['js']);
});

gulp.task('watch-less', function () {
    gulp.watch("./src/blocks/**/*.less", ['less']);

});

gulp.task('dev-start', [
    'less',
    'js',
    'watch-less',
    'watch-js'
]);