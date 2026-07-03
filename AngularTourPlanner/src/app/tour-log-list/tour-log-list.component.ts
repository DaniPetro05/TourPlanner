import { Component } from '@angular/core';
import { Input } from '@angular/core';
import { NgIf, NgFor, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SimpleChanges } from '@angular/core';
import { TourLog } from '../models/tour-log';
import { TourLogService } from '../services/tour-log.service';

@Component({
  selector: 'app-tour-log-list',
  imports: [NgIf, NgFor, FormsModule, DatePipe],
  templateUrl: './tour-log-list.component.html',
  styleUrl: './tour-log-list.component.css'
})
export class TourLogListComponent {
  @Input() tourId?: number;

  constructor(private logService: TourLogService) {}

  logs: TourLog[] = [];

  loadLogs() {
    if (!this.tourId) {
      this.logs = [];
      return;
    }

    this.logService.getLogsByTour(this.tourId).subscribe(logs => this.logs = logs);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['tourId']) {
      this.loadLogs();
    }
  }

  showForm = false;

  newLog: TourLog = {
    id: 0,
    tourId: 0,
    date: '',
    comment: '',
    difficulty: 0,
    totalDistance: 0,
    totalTime: 0,
    rating: 1
  };

  toggleForm() {
    this.showForm = !this.showForm;
  }

  saveLog() {
    if (!this.tourId) return;

    const dto = {
      //date: this.newLog.date,
      date: new Date().toISOString(), //doesn't actually do anything, as date is set in Backend, but left as is due to time constraints
      comment: this.newLog.comment,
      difficulty: this.newLog.difficulty,
      totalDistance: this.newLog.totalDistance,
      totalTime: this.newLog.totalTime,
      rating: this.newLog.rating,
      tourId: this.tourId
    };

    if (this.editingLogId) {
      this.logService.updateLog(this.editingLogId, dto).subscribe(() => {
            this.loadLogs();
        });
    }
    else {
      this.logService.createLog(dto).subscribe({
        next: () => this.loadLogs(),
        error: err => console.error("Create log failed:", err)
      });
    }

    this.showForm = false;

    this.editingLogId = undefined;

    this.newLog = {
        id: 0,
        tourId: 0,
        date: '',
        comment: '',
        difficulty: 0,
        totalDistance: 0,
        totalTime: 0,
        rating: 1
    };
  }

  deleteLog(id: number) {
    this.logService.deleteLog(id).subscribe(() => this.loadLogs());
    this.loadLogs();
  }

  editingLogId?: number;

  editLog(log: TourLog) {
    this.showForm = true;
    this.editingLogId = log.id;

    this.newLog = { ...log };
  }
}
