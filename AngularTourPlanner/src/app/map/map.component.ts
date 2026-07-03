import { Component, AfterViewInit, Input, OnChanges } from '@angular/core';
import * as L from 'leaflet';
import { Tour } from '../models/tour';

@Component({
  selector: 'app-map',
  imports: [],
  templateUrl: './map.component.html',
  styleUrl: './map.component.css'
})
export class MapComponent implements AfterViewInit, OnChanges {
  @Input() tour?: Tour;
  
  private map: any;

  ngAfterViewInit(): void {
    this.initMap();
    this.updateMap();
  }

  ngOnChanges(): void {
    if (!this.map || !this.tour?.routeGeometry)
      return;
    
    this.updateMap();
  }

  private initMap(): void {
    this.map = L.map('map').setView([48.2082, 16.3738], 13);  //Vienna

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    }).addTo(this.map);
  }

  private routeLayer: any;

  updateMap() {
    if (!this.tour?.routeGeometry)
      return;

    const geometry = JSON.parse(this.tour.routeGeometry!);

    const latLngs = geometry.coordinates.map(
      (c: number[]) => [c[1], c[0]]
    );

    if (this.routeLayer) {
        this.map.removeLayer(this.routeLayer);
    }

    this.routeLayer = L.polyline(latLngs, {
        color: 'blue',
        weight: 5
    }).addTo(this.map);

    this.map.fitBounds(this.routeLayer.getBounds());
  }
}
