import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { UsersComponent } from './users/users.component';
import { HomeComponent } from './home/home.component';
import { ToursComponent } from './tours/tours.component';

export const routes: Routes = [
 { path: 'user', component: UsersComponent },
 { path: 'tours', component: ToursComponent },
 
 { path: 'home', component: HomeComponent },
 { path: '**', redirectTo: '/home'}
];
