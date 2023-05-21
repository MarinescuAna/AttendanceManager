<template>
  <VueApexCharts
    width="1500"
    type="bar"
    class="ma-3"
    :options="chartOptions"
    :series="chartData"
  ></VueApexCharts>
</template>

<script lang="ts">
import Vue from "vue";
import VueApexCharts from "vue-apexcharts";

export default Vue.extend({
  name: "BarChartComponent",
  components: { VueApexCharts },
  props: {
    /** Data to display in the bar chart*/
    chartData: {
      type: Array,
      required: true,
      default: () => []
    },
    /** Data to display in the bar chart*/
    xAxiesLabels: {
      type: Array,
      required: true,
      default: () => []
    },
    /** The title of the bar chart */
    title: {
      type: String,
      required: true,
    },
    /** Display the chart on horizontal */
    horizontal: {
      type: Boolean,
      default: false,
    }
  },
  data: function () {
    return {
      chartOptions: {
        title: {
          text: this.title,
        },
        plotOptions: {
          bar: {
            horizontal: this.horizontal,
          }
        },
        xaxis: {
          categories: this.xAxiesLabels,
        },
      },
    };
  },
});
</script>