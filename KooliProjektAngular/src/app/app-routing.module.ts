import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ProjectsComponent } from './components/projects/projects.component';
import { ProjectEditComponent } from './components/project-edit/project-edit.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'projects/edit/:id', component: ProjectEditComponent },
  { path: '**', redirectTo: '' }
];

