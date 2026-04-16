import { Component } from '@angular/core';
import { Input } from '@angular/core';
import { NgIf, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SimpleChanges } from '@angular/core';
import { TourLog } from '../tour-log';

@Component({
  selector: 'app-tour-log-list',
  imports: [NgIf, NgFor, FormsModule],
  templateUrl: './tour-log-list.component.html',
  styleUrl: './tour-log-list.component.css'
})
export class TourLogListComponent {
  @Input() tourId?: number;

  logs: TourLog[] = [];

  /*ngOnChanges() {
    if (this.tourId) {
      //TEMPORARY: mock data
      this.logs = [
        { id: 1, tourId: this.tourId, date: '2026-01-01', comment: 'Nice', rating: 4 },
      ];
    }
  }*/

  allLogs: TourLog[] = [
    { id: 1, tourId: 1, date: '2026-01-01', comment: 'Nice', rating: 4 },
    { id: 2, tourId: 2, date: '2026-01-02', comment: 'Great', rating: 5 }
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['tourId']) {
      this.loadLogs();
    }
  }

  loadLogs() {
    if (!this.tourId) {
      this.logs = [];
      return;
    }

    this.logs = this.allLogs.filter(
      log => log.tourId === this.tourId
    );
  }

  showForm = false;

  newLog: TourLog = {
    id: 0,
    tourId: 0,
    date: '',
    comment: '',
    rating: 1
  };

  toggleForm() {
    this.showForm = !this.showForm;
  }

  /*addLog() {
    if (!this.tourId) return;

    const newEntry: TourLog = {
      ...this.newLog,
      id: Date.now(), // simple unique ID
      tourId: this.tourId
    };

    this.logs.push(newEntry);

    //reset form
    this.newLog = {
      id: 0,
      tourId: 0,
      date: '',
      comment: '',
      rating: 1
    };

    this.showForm = false;
  }*/

  saveLog() {
    if (!this.tourId) return;

    if (this.editingLogId) {
      const log = this.allLogs.find(l => l.id === this.editingLogId);
      if (log) {
        log.date = this.newLog.date;
        log.comment = this.newLog.comment;
        log.rating = this.newLog.rating;
      }

      this.editingLogId = undefined;

    } else {
      const newEntry: TourLog = {
        ...this.newLog,
        id: Date.now(),
        tourId: this.tourId
      };

      this.allLogs.push(newEntry);
    }

    this.loadLogs();
    this.showForm = false;
  }

  deleteLog(id: number) {
    this.allLogs = this.allLogs.filter(log => log.id !== id);
    this.loadLogs();
  }

  editingLogId?: number;

  editLog(log: TourLog) {
    this.showForm = true;
    this.editingLogId = log.id;

    this.newLog = { ...log };
  }
}
