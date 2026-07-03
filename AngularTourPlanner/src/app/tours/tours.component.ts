import { Component } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ToolbarComponent } from '../toolbar/toolbar.component';
import { TourDetailComponent } from '../tour-detail/tour-detail.component';

import { Tour } from '../models/tour';
import { TourService } from '../services/tour.service';

@Component({
  selector: 'app-tours',
  imports: [ToolbarComponent, TourDetailComponent, NgIf, NgFor, FormsModule],
  templateUrl: './tours.component.html',
  styleUrl: './tours.component.css'
})
export class ToursComponent {

  tours: Tour[] = [];

  selectedTour?: Tour;

  showForm = false;

  editingTourId?: number;

  transportTypes: string[] = [];

  newTourForm = {
    name: '',
    description: '',
    from: '',
    to: '',
    transportType: '',
    distance: 0,
    estimatedTime: '',
    imagePath: '',
    stopsString: ''
  };

  constructor(private tourService: TourService) {}

  ngOnInit() {
    this.loadTours();

    this.tourService.getTransportTypes().subscribe(types => {
        this.transportTypes = types;
    });
  }

  loadTours() {
    this.tourService.getTours().subscribe(data => {
      this.tours = data;
    });
  }

  onSelect(tour: Tour) {
    this.selectedTour = tour;
  }

  toggleForm() {
    this.showForm = !this.showForm;
  }

  saveTour() {
    const stops = this.newTourForm.stopsString
      ? this.newTourForm.stopsString.split(',').map(s => s.trim())
      : [];

    const tour: Tour = {
      name: this.newTourForm.name,
      description: this.newTourForm.description,
      from: this.newTourForm.from,
      to: this.newTourForm.to,
      transportType: this.newTourForm.transportType,
      distance: this.newTourForm.distance,
      estimatedTime: this.newTourForm.estimatedTime,
      imagePath: this.newTourForm.imagePath,
      stops
    };

    if (this.editingTourId !== undefined && this.editingTourId !== null) {
      // EDIT
      this.tourService.updateTour(this.editingTourId, tour).subscribe({
          next: () => {
              this.loadTours();
              this.showForm = false;
          },
          error: err => {
              alert(err.error.error);
          }
      });
    } else {
      // CREATE
      this.tourService.createTour(tour).subscribe({
          next: () => {
              this.loadTours();
              this.showForm = false;
          },
          error: err => {
              alert(err.error.error);
          }
      });
    }

    this.showForm = false;
    this.editingTourId = undefined;

    this.newTourForm = {
      name: '',
      description: '',
      from: '',
      to: '',
      transportType: '',
      distance: 0,
      estimatedTime: '',
      imagePath: '',
      stopsString: ''
    };
  }

  deleteTour(id: number) {
    this.tourService.deleteTour(id).subscribe(() => {
      this.loadTours();

      if (this.selectedTour?.id === id) {
        this.selectedTour = undefined;
      }
    });
  }

  editTour(tour: Tour) {
    this.showForm = true;
    this.editingTourId = tour.id;

    this.newTourForm = {
      name: tour.name,
      description: tour.description,
      from: tour.from,
      to: tour.to,
      transportType: tour.transportType,
      distance: tour.distance,
      estimatedTime: tour.estimatedTime,
      imagePath: tour.imagePath || '',
      stopsString: tour.stops?.join(', ') || ''
    };
  }

  searchQuery = '';

  search() {
    this.tourService.searchTours(this.searchQuery).subscribe(data => {
      this.tours = data;
    });
  }

  downloadExport() {
    this.tourService.exportTours().subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'tours.json';
      a.click();
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];

    const reader = new FileReader();

    reader.onload = () => {
      const json = JSON.parse(reader.result as string);

      this.tourService.importTours(json).subscribe(() => {
        console.log("Import successful");
        this.loadTours(); // refresh list
      });
    };

    reader.readAsText(file);
  }
}