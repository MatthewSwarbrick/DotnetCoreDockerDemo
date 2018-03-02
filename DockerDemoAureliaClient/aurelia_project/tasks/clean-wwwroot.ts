import * as gulp from 'gulp';
import * as clean from 'gulp-clean';

export default function cleanWwwRoot() {
    return gulp.src('../DockerDemoApi/DockerDemoApi/wwwroot/*').pipe(clean({ force: true }));
}