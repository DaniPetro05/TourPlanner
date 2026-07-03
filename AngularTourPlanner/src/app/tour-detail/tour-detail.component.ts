/*import { Component, Input } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { MapComponent } from '../map/map.component';
import { Tour } from '../tour';
import { TourLog } from '../tour-log';
import { TourLogService } from '../services/tour-log.service';

@Component({
  selector: 'app-tour-detail',
  imports: [MapComponent, NgIf, NgFor],
  templateUrl: './tour-detail.component.html',
  styleUrl: './tour-detail.component.css'
})
export class TourDetailComponent {

  @Input() tour?: Tour;

  logs: TourLog[] = [];

  constructor(private logService: TourLogService) {}

  ngOnChanges() {
    if (this.tour?.id) {
      this.loadLogs(this.tour.id);
    }
  }

  loadLogs(tourId: number) {
    this.logService.getLogsByTour(tourId)
      .subscribe(data => this.logs = data);
  }
}*/

import { Component, Input } from '@angular/core';
import { NgIf, NgFor, CommonModule } from '@angular/common';

import { MapComponent } from '../map/map.component';
import { TourLogListComponent } from '../tour-log-list/tour-log-list.component';

import { Tour } from '../models/tour';

@Component({
  selector: 'app-tour-detail',
  imports: [
    MapComponent,
    TourLogListComponent,
    NgIf,
    NgFor,
    CommonModule
  ],
  templateUrl: './tour-detail.component.html',
  styleUrl: './tour-detail.component.css'
})
export class TourDetailComponent {

  @Input()
  tour?: Tour;

}