import { Component, Input } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { MapComponent } from '../map/map.component';
import { TourLogListComponent } from '../tour-log-list/tour-log-list.component';
import { Tour } from '../tour';

@Component({
  selector: 'app-tour-detail',
  imports: [MapComponent, TourLogListComponent, NgIf, NgFor],
  templateUrl: './tour-detail.component.html',
  styleUrl: './tour-detail.component.css'
})
export class TourDetailComponent {
  @Input() tour?: Tour;
}
