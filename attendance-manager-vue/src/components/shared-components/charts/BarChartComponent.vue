<template>
  <Bar class="bar-chart-size" :options="chartOptions" :data="chartData" />
</template>

<style lang="css">
.bar-chart-size {
  width: 70% !important;
  height: 70% !important;
}
</style>

<script lang="ts">
import Vue from "vue";
import { Bar } from "vue-chartjs";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  Colors,
} from "chart.js";

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  Colors
);

export default Vue.extend({
  name: "BarChartComponent",
  components: { Bar },
  props: {
    /** Data to display in the bar chart*/
    chartData: {
      type: Object,
      required: true,
    },
    /** The title of the bar chart */
    title: {
      type: String,
      required: true,
    }
  },
  data: function () {
    return {
      /** Use this to show the loader */
      isLoading: true,
      chartOptions: {
        //  A boolean value that specifies whether the chart should be responsive to changes in its container's size
        responsive: true,
        // Set up the title of the chart
        plugins: {
          title: {
            display: true,
            text: this.title,
            fontSize: 20,
          },
        },
      },
    };
  },
});
</script>