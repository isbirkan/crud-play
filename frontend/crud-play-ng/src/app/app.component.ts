import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { filter, map } from 'rxjs/operators';

import { LoadingOverlayComponent } from '@/app/components/loading-overlay/loading-overlay.component';
import { ToastComponent } from '@/app/components/toast/toast.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoadingOverlayComponent, ToastComponent],
  template: `
    <router-outlet></router-outlet>
    <app-loading-overlay></app-loading-overlay>
    <app-toast></app-toast>
  `
})
export class AppComponent {
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title
  ) {
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        map(() => {
          let route = this.activatedRoute.firstChild;
          while (route?.firstChild) {
            route = route.firstChild;
          }
          return route?.snapshot.data['title'] || 'crud-play';
        })
      )
      .subscribe((title) => {
        this.titleService.setTitle(title);
      });
  }
}
