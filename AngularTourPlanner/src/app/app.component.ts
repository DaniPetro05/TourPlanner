import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';
import { UserService, User } from './user.service';
import { HttpClient } from '@angular/common/http';
import { signal } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
//import { ToolbarComponent } from './toolbar/toolbar.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet/*, ToolbarComponent*/],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AngularTourPlanner';
}
