import { Component } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToolbarComponent } from '../toolbar/toolbar.component';
import { TourDetailComponent } from '../tour-detail/tour-detail.component';
import { Tour } from '../tour';

@Component({
  selector: 'app-tours',
  imports: [ToolbarComponent, TourDetailComponent, NgIf, NgFor, FormsModule],
  templateUrl: './tours.component.html',
  styleUrl: './tours.component.css'
})
export class ToursComponent {
  tours: Tour[] = [
  {
    id: 1,
    name: 'Vienna City Tour',
    description: 'Explore Vienna',
    from: 'Hauptbahnhof Wien',
    to: 'Favoriten',
    stops: [
      'Simmering',
      'Leopoldstadt',
      'Donaustadt',
      'Floridsdorf',
      'Brigittenau',
      'Döbling',
      'Währing',
      'Hernals',
      'Ottakring',
      'Rudolfheim-Fünfhaus',
      'Penzing',
      'Hietzing',
      'Liesing',
      'Meidling',
      'Margareten Wien',
      'Mariahilf',
      'Neubau',
      'Josefstadt Wien',
      'Alsergrund',
      'Innere Stadt',
      'Wieden']
    //lat: 48.2082,
    //lng: 16.3738
  },
  {
    id: 2,
    name: 'Austria Tour',
    description: 'Travel Austria',
    from: 'Wien Hauptbahnhof',
    to: 'Wien Favoriten',
    stops: [
      'Burgenland',
      'Steiermark',
      'Kärnten',
      'Osttirol',
      'Tirol',
      'Vorarlberg',
      'Tirol',
      'Salzburg',
      'Oberösterreich',
      'Niederösterreich',
    ]
    //lat: 48.251,
    //lng: 16.405
  }
];

  selectedTour?: Tour;

  onSelect(tour: Tour) {
    this.selectedTour = tour;
  }

  //can be used for the first tour to be automatically selected
  /*ngOnInit() {
    if (this.tours.length > 0) {
      this.selectedTour = this.tours[0];
    }
  }*/

  showForm = false;

  newTour: Tour = {
    id: 0,
    name: '',
    description: '',
    from: '',
    to: '',
    imagePath: ''
  };

  toggleForm() {
    this.showForm = !this.showForm;
  }

  newTourForm = {
    name: '',
    description: '',
    imagePath: '',
    from: '',
    to: '',
    stopsString: ''
  };

  /*addTour() {
    if (!this.newTour.name || !this.newTour.description) return;

    const tourToAdd: Tour = {
      ...this.newTour,
      id: Date.now()  //simple unique ID
    };

    this.tours.push(tourToAdd);

    //reset form
    this.newTour = {
      id: 0,
      name: '',
      description: '',
      imagePath: ''
    };

    this.showForm = false;
  }*/

  /*addTour() {
    const stops = this.newTourForm.stopsString
      ? this.newTourForm.stopsString.split(',').map((s: string) => s.trim())
      : [];

    const tourToAdd: Tour = {
      id: Date.now(),
      name: this.newTourForm.name,
      description: this.newTourForm.description,
      imagePath: this.newTourForm.imagePath,
      from: this.newTourForm.from,
      to: this.newTourForm.to,
      stops
    };

    this.tours.push(tourToAdd);

    this.newTourForm = {
      name: '',
      description: '',
      imagePath: '',
      from: '',
      to: '',
      stopsString: ''
    };
  }*/

  saveTour() {
    const stops = this.newTourForm.stopsString
      ? this.newTourForm.stopsString.split(',').map((s: string) => s.trim())
      : [];

    if (this.editingTourId) {
      // EDIT
      const tour = this.tours.find(t => t.id === this.editingTourId);
      if (tour) {
        tour.name = this.newTourForm.name;
        tour.description = this.newTourForm.description;
        tour.imagePath = this.newTourForm.imagePath;
        tour.from = this.newTourForm.from;
        tour.to = this.newTourForm.to;
        tour.stops = stops;
      }

      this.editingTourId = undefined;

    } else {
      // CREATE
      const newTour: Tour = {
        id: Date.now(),
        name: this.newTourForm.name,
        description: this.newTourForm.description,
        imagePath: this.newTourForm.imagePath,
        from: this.newTourForm.from,
        to: this.newTourForm.to,
        stops
      };

      this.tours.push(newTour);
    }

    this.showForm = false;
  }

  deleteTour(id: number) {
    this.tours = this.tours.filter(t => t.id !== id);

    //clear selection if deleted
    if (this.selectedTour?.id === id) {
      this.selectedTour = undefined;
    }
  }

  editingTourId?: number;

  editTour(tour: Tour) {
    this.showForm = true;
    this.editingTourId = tour.id;

    this.newTourForm = {
      name: tour.name,
      description: tour.description,
      imagePath: tour.imagePath || '',
      from: tour.from,
      to: tour.to,
      stopsString: tour.stops?.join(', ') || ''
    };
  }
}
