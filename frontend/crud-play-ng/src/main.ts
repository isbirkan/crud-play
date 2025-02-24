/* eslint-disable @typescript-eslint/no-empty-function */
import { bootstrapApplication } from '@angular/platform-browser';

import { environment } from '@/environments/environment';

import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';

if (environment.production) {
  console.log = function () {};
  console.info = function () {};
}

bootstrapApplication(AppComponent, appConfig).catch((err) => console.error(err));
