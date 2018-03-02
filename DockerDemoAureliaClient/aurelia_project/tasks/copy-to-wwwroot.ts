import * as gulp from 'gulp';

export default function copyToWwwRoot() {
    return gulp.src(['scripts/**/*', 'assets/**/*', 'index.html'],{
                base: './'
            })
            .pipe(gulp.dest('../DockerDemoApi/DockerDemoApi/wwwroot/'));
}