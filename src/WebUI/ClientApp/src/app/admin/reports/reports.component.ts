import { Component, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { ReportsClient } from '../../rentasgt-api';

const MONTHS: string[] = [
  '',
  'Enero',
  'Febrero',
  'Marzo',
  'Abril',
  'Mayo',
  'Junio',
  'Julio',
  'Agosto',
  'Septiembre',
  'Octubre',
  'Noviembre',
  'Diciembre'
];

const MONTHS_DICT: any = {
  '': 0,
  'Enero': 1,
  'Febrero': 2,
  'Marzo': 3,
  'Abril': 4,
  'Mayo': 5,
  'Junio': 6,
  'Julio': 7,
  'Agosto': 8,
  'Septiembre': 9,
  'Octubre': 10,
  'Noviembre': 11,
  'Diciembre': 12
};

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {

  public loadingYearReport: boolean = true;
  public yearReportChartData: ChartDataSets[] = [];
  public yearReportChartLabels: Label[] = [];
  public yearRerportChartOptions: ChartOptions = {
    responsive: true
  };
  public yearReportChartColors: Color[] = [
    {
      borderColor: 'black',
      backgroundColor: 'rgba(255,0,0,0.3)',
    },
  ];
  public yearReportChartLegend = true;
  public yearReportChartType = 'bar';

  public loadingMonthReport: boolean = true;
  public monthReportChartData: ChartDataSets[] = [];
  public monthReportChartLabels: Label[] = [];
  public monthRerportChartOptions: ChartOptions = {
    responsive: true
  };
  public monthReportChartColors: Color[] = [
    {
      borderColor: 'black',
      backgroundColor: 'rgba(255,0,0,0.3)',
    },
  ];
  public monthReportChartLegend = true;
  public monthReportChartType = 'bar';

  public loadingDayReport: boolean = true;
  public dayReportChartData: ChartDataSets[] = [];
  public dayReportChartLabels: Label[] = [];
  public dayRerportChartOptions: ChartOptions = {
    responsive: true
  };
  public dayReportChartColors: Color[] = [
    {
      borderColor: 'black',
      backgroundColor: 'rgba(255,0,0,0.3)',
    },
  ];
  public dayReportChartLegend = true;
  public dayReportChartType = 'bar';

  public selectedYear: number = null;
  public selectedMonth: number = null;

  constructor(
    private reportsClient: ReportsClient,
  ) { }

  ngOnInit(): void {
    this.loadYearReport();
  }

  private loadYearReport(): void {
    this.reportsClient.getRentsByYear()
      .subscribe((res) => {
        this.yearReportChartLabels = [];
        this.yearReportChartLabels = res.results.map(r => '' + r.year);
        this.yearReportChartData = [
          {
            data: res.results.map(r => r.total),
            label: 'Rentas por año'
          }
        ];
        this.loadingYearReport = false;
      }, console.error);
  }

  public onYearClicked($event: any): void {
    if ($event.active.length > 0) {
      const ano = this.yearReportChartLabels[$event.active[0]._index].toString();
      if (ano) {
        this.selectedYear = Number.parseInt(ano);
        this.loadMonthReport(this.selectedYear);
      }
    }
  }

  private loadMonthReport(year: number): void {
    this.reportsClient.getRentsByMonth(year)
      .subscribe((res) => {
        this.monthReportChartLabels = [];
        this.monthReportChartLabels = res.results.map(r => '' + MONTHS[r.month]);
        this.monthReportChartData = [
          {
            data: res.results.map(r => r.total),
            label: `${year}`
          }
        ];
        this.loadingMonthReport = false;
      }, console.error);
  }

  public onMonthClicked($event): void {
    if ($event.active.length > 0) {
      const month = MONTHS_DICT[this.monthReportChartLabels[$event.active[0]._index].toString()];
      this.selectedMonth = month;
      this.loadDayReport(this.selectedYear, month);
    }
  }

  private loadDayReport(year: number, month: number): void {
    this.reportsClient.getRentsByDay(year, month)
      .subscribe((res) => {
        this.dayReportChartLabels = [];
        this.dayReportChartLabels = res.results.map(r => ('0' + r.day).substr(-2));
        this.dayReportChartData = [
          {
            data: res.results.map(r => r.total),
            label: `${MONTHS[month]}, ${year}`
          }
        ];
        this.loadingDayReport = false;
      }, console.error);
  }

}
