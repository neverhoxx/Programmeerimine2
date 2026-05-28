import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../services/project.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html'
})
export class ProjectsComponent implements OnInit {
  projects: any[] = [];

  constructor(private service: ProjectService) {}

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.service.list().subscribe(resp => {
      if (resp && resp.value && resp.value.results) {
        this.projects = resp.value.results;
      } else if (Array.isArray(resp)) {
        this.projects = resp;
      } else if (resp && resp.results) {
        this.projects = resp.results;
      }
    });
  }
}
