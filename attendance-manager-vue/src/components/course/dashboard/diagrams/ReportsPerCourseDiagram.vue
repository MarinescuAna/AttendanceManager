<template>
  <MessageComponent
    icon="mdi-information-variant-circle-outline"
    description="<strong>There is no data!</strong>"
    :color="WARNING_AMBER_DARKEN_4"
    v-if="chartDataLables.length == 0"
  />
  <v-layout column v-else-if="isMobile">
    <TitleWithInfoComponent :title="TITLE" :description="DESCRIPTION" h_Text="2"/>
    <BarChartComponent :values="chartDataValues" :labels="chartDataLables" :horizontal="true" class="move-behind"/>
  </v-layout>
  <v-layout column v-else>
    <h2>{{ TITLE }}</h2>
    <v-flex class="d-flex flex-row">
      <div class="custom-box ma-6 my-12">
        <p>{{ DESCRIPTION }}</p>
        <v-img
          :src="require(`@/assets/images/diagrams/compute_reports.svg`)"
          class="ma-3"
          width="150px"
        ></v-img>
      </div>
      <BarChartComponent :values="chartDataValues" :labels="chartDataLables" :horizontal="true" class="move-behind"/>
    </v-flex>
  </v-layout>
</template>
<style scoped>
.custom-box {
  width: 20%;
}
.move-behind {
  z-index: 0;
}
</style>

  <script lang="ts">
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import TitleWithInfoComponent from "@/components/shared-components/TitleWithInfoComponent.vue";
import BarChartComponent from "@/components/shared-components/charts/BarChartComponent.vue";
import Vue from "vue";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import { CourseDashboardViewModule } from "@/modules/view-modules";

export default Vue.extend({
  name: "ReportsPerCourseDiagram",
  components: { TitleWithInfoComponent, BarChartComponent, MessageComponent },
  props: {
    courses: {
      type: Array as () => CourseDashboardViewModule[],
      required: true,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      DESCRIPTION:
        "The diagram shows the number of reports defined for each course.",
      TITLE: "Total reports defined per course",
      chartDataLables: [] as string[],
      chartDataValues: {},
    };
  },
  computed: {
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.mdAndDown;
    },
  },
  created: function (): void {
    const result = this._countReports();

    if (result != null) {
      this.chartDataLables = result.labels;
      this.chartDataValues = [
        {
          name:"Number of reports",
          data: result.values,
        },
      ];
    }
  },
  methods: {
    _countReports: function (): {
      labels: string[];
      values: number[];
    } {
      let result: {
        labels: string[];
        values: number[];
      } = {
        labels: [],
        values: [],
      };

      this.courses.forEach((course) => {
        result.labels.push(course.courseName);
        result.values.push(course.reports.length);
      });
      return result;
    },
  },
});
</script>