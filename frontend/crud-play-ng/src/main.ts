/* eslint-disable @typescript-eslint/no-empty-function */
import '@angular/compiler';
import { bootstrapApplication } from '@angular/platform-browser';

import { AppComponent } from '@/app/app.component';
import { appConfig } from '@/app/app.config';
import { environment } from '@/environments/environment';

if (environment.production) {
  console.log = function () {};
  console.info = function () {};
}

bootstrapApplication(AppComponent, appConfig).catch((err) => console.error(err));
