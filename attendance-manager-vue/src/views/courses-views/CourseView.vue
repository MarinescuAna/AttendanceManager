<template>
    <v-layout  justify-center column>
          <v-btn class="orange lighten-3 ma-3" :to="{ name: 'create-course' }"
            >Add new course</v-btn
          >
        <router-view></router-view>
        <ManagementTableComponent
          :dataSource="courses"
          :headers="headers"
          title="Courses"
          :type="type"
          :expandDetails="false"
          displayMessage="There is no course defined yet."
        />
    </v-layout>
</template>

<script lang="ts">
import Vue from "vue";
import ManagementTableComponent from "@/components/shared-components/ManagementTableComponent.vue";
import storeHelper from "@/store/store-helper";
import { CoursesHeader } from "@/components/shared-components/Headers";
import { ManagementDataType } from "@/shared/enums";
import { CourseViewModule } from "@/modules/course";

export default Vue.extend({
  name: "CourseView",
  components: {
    ManagementTableComponent,
  },
  data: function () {
    return {
      headers: CoursesHeader,
      type: ManagementDataType.Course,
    };
  },
  computed: {
    courses: function (): CourseViewModule[] {
      return storeHelper.courseStore.courses;
    },
  },
  created: async function () {
    await storeHelper.courseStore.loadCourses();
    await storeHelper.userStore.loadCurrentUserInfo();
  },
});
</script>