import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import { LoadingService } from '@/app/services/loading/loading.service';

@Component({
  selector: 'app-loading-overlay',
  imports: [CommonModule],
  templateUrl: './loading-overlay.component.html'
})
export class LoadingOverlayComponent {
  constructor(public loadingService: LoadingService) {}
}
