import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../services/project.service';

@Component({
  selector: 'app-project-edit',
  templateUrl: './project-edit.component.html'
})
export class ProjectEditComponent implements OnInit {
  form: FormGroup;
  id = 0;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: ProjectService
  ) {
    this.form = this.fb.group({
      id: [0],
      name: [''],
      startDate: [''],
      dueDate: [''],
      budget: [0],
      pricePerHour: [0]
    });
  }

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id')) || 0;
    if (this.id !== 0) {
      this.service.get(this.id).subscribe((resp: any) => {
        const value = resp?.value || resp;
        if (value) {
          this.form.patchValue(value);
        }
      });
    }
  }

  save() {
    this.service.save(this.form.value).subscribe(() => {
      this.router.navigate(['/projects']);
    });
  }
}
