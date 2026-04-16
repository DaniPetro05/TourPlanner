import { Component, AfterViewInit, Input, OnChanges } from '@angular/core';
import * as L from 'leaflet';
import { Tour } from '../tour';
//import { ToursComponent } from '../tours/tours.component';

/*import markerIcon from '../../../node_modules/leaflet/dist/images/marker-icon.png';
L.Marker.prototype.setIcon(L.icon({
  iconUrl:markerIcon
}))*/

/*delete (L.Icon.Default.prototype as any)._getIconUrl;

L.Icon.Default.mergeOptions({
  iconRetinaUrl: '/../assets/marker-icon-2x.png',
  iconUrl: '/../assets/marker-icon.png',
  shadowUrl: '/../assets/marker-shadow.png',
});*/

const viennaCoords: Record<string, [number, number]> = {
  "Hauptbahnhof Wien": [48.185, 16.374],
  "Favoriten": [48.174, 16.377],
  "Simmering": [48.173, 16.442],
  "Leopoldstadt": [48.216, 16.400],
  "Donaustadt": [48.233, 16.450],
  "Floridsdorf": [48.255, 16.400],
  "Innere Stadt": [48.208, 16.373]
};

@Component({
  selector: 'app-map',
  imports: [],
  templateUrl: './map.component.html',
  styleUrl: './map.component.css'
})
export class MapComponent implements AfterViewInit, OnChanges {
  @Input() tour?: Tour;
  
  private map: any;
  private marker: any;

  ngAfterViewInit(): void {
    this.initMap();
    this.updateMap();
  }

  ngOnChanges(): void {
    if (this.map && this.tour?.name && this.tour?.from && this.tour?.to && this.tour.stops/* && this.tour?.lat && this.tour?.lng*/) {
      this.updateMap();

      console.log("Map updated!");
    }
  }

  private initMap(): void {
    this.map = L.map('map').setView([48.2082, 16.3738], 13);  //Vienna

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    }).addTo(this.map);

    //delete (L.Icon.Default.prototype as any)._getIconUrl;

    /*L.Icon.Default.mergeOptions({
      iconRetinaUrl: './marker-icon-2x.png',
      iconUrl: './marker-icon.png',
      shadowUrl: './marker-shadow.png',
    });*/

    /*L.Icon.Default.mergeOptions({
      iconRetinaUrl: 'leaflet/dist/images/marker-icon-2x.png',
      iconUrl: 'leaflet/dist/images/marker-icon.png',
      shadowUrl: 'leaflet/dist/images/marker-shadow.png',
    });*/

    //console.log((L.Icon.Default.prototype as any)._getIconUrl);
  }

  /*private updateMap(): void {
    const coords: [number, number] = [this.tour!.lat!, this.tour!.lng!];

    //Move map
    this.map.setView(coords, 13);

    //Remove old marker
    if (this.marker) {
      this.map.removeLayer(this.marker);
    }

    //Add new marker
    this.marker = L.marker(coords).addTo(this.map)
      .bindPopup(this.tour?.name || '')
      .openPopup();
  }*/

  private routeLayer: any;

  /*async updateMap() {
    if (!this.tour) return;

    const fromCoords = await this.getCoordinates(this.tour.from);
    const toCoords = await this.getCoordinates(this.tour.to);

    if (!fromCoords || !toCoords) return;

    const points: [number, number][] = [
      fromCoords,
      toCoords
    ];

    // Remove old route
    if (this.routeLayer) {
      this.map.removeLayer(this.routeLayer);
    }

    // Draw route
    this.routeLayer = L.polyline(points, { color: 'blue' }).addTo(this.map);

    this.map.fitBounds(this.routeLayer.getBounds());
  }*/

  /*async updateMap() {
    if (!this.tour) return;

    const points: [number, number][] = [];

    //from
    const from = await this.getCoordinates(this.tour.from);
    if (from) points.push(from);

    //stops
    if (this.tour.stops) {
      for (const stop of this.tour.stops) {
        const coords = await this.getCoordinates(stop);
        if (coords) points.push(coords);
      }
    }

    //to
    const to = await this.getCoordinates(this.tour.to);
    if (to) points.push(to);

    //draw route
    if (this.routeLayer) {
      this.map.removeLayer(this.routeLayer);
    }

    this.routeLayer = L.polyline(points, { color: 'blue' }).addTo(this.map);

    this.map.fitBounds(this.routeLayer.getBounds());
  }*/

  async updateMap() {
    if (!this.tour) return;

    const points: [number, number][] = [];

    const from = await this.getCoordinates(this.tour.from);
    if (from) points.push(from);

    if (this.tour.stops) {
      for (const stop of this.tour.stops) {
        const coords = await this.getCoordinates(stop);
        if (coords) points.push(coords);
      }
    }

    const to = await this.getCoordinates(this.tour.to);
    if (to) points.push(to);

    if (points.length < 2) return;

    const coordsString = points
      .map(p => `${p[1]},${p[0]}`)
      .join(';');

    const res = await fetch(
      `https://router.project-osrm.org/route/v1/driving/${coordsString}?overview=full&geometries=geojson`
    );

    const data = await res.json();

    if (!data.routes || data.routes.length === 0) return;

    const routeCoords = data.routes[0].geometry.coordinates;

    const latLngs: [number, number][] = routeCoords.map(
      (c: [number, number]) => [c[1], c[0]]
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

  /*async getCoordinates(place: string): Promise<[number, number] | null> {
    const res: any = await fetch(
      `https://nominatim.openstreetmap.org/search?format=json&q=${place}`
    );

    const data = await res.json();

    if (data.length === 0) return null;

    return [parseFloat(data[0].lat), parseFloat(data[0].lon)];
  }*/

  async getCoordinates(place: string): Promise<[number, number] | null> {
    if (viennaCoords[place]) {
      return viennaCoords[place];
    }

    const res = await fetch(
      `https://photon.komoot.io/api/?q=${encodeURIComponent(place)}`
    );

    const data = await res.json();

    if (!data.features || data.features.length === 0) return null;

    const coords = data.features[0].geometry.coordinates;

    //Photon returns [lon, lat]; Leaflet needs [lat, lon]
    return [coords[1], coords[0]];
  }
}
