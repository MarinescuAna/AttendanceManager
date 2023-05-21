<template>
  <v-layout column>
    <v-flex class="one-row">
      <h3>{{ title }}</h3>
      <v-tooltip top>
        <template v-slot:activator="{ on, attrs }">
          <v-icon v-bind="attrs" v-on="on" class="ma-1"
            >mdi-alert-circle</v-icon
          >
        </template>
        <span v-html="description == '' ? title : description"></span>
      </v-tooltip>
    </v-flex>
    <VueApexCharts
      width="1000"
      type="pie"
      class="ma-3"
      :options="chartOptions"
      :series="series"
    ></VueApexCharts>
  </v-layout>
</template>

<style scoped>
.one-row {
  display: -webkit-box;
}
</style>   

<script lang="ts">
import Vue from "vue";
import VueApexCharts from "vue-apexcharts";

export default Vue.extend({
  name: "PieChartComponent",
  components: { VueApexCharts },
  props: {
    /** Data to display in the pie chart*/
    dataSeries: {
      type: Array,
      //required: true,
      default: () => [],
    },
    /** Data to display in the pie chart*/
    labels: {
      type: Array,
      //required: true,
      default: () => [],
    },
    /** The title of the pie chart */
    title: {
      type: String,
      required: true,
    },
    /**Explanatio regrding the current data from the diagram */
    description: {
      type: String,
      default: "",
    },
  },
  data: function () {
    return {
      series: this.dataSeries,
      chartOptions: {
        chart: {
          type: "pie",
        },
        labels: this.labels,
        legend: {
        position: 'bottom',
        fontSize: '14px' // Adjust the font size as desired
      }
      },
    };
  },
});
</script>