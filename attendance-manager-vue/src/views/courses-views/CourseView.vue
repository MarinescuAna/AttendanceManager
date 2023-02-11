<template>
  <v-container>
    <v-row justify="center">
      <v-btn-toggle>
        <v-btn class="blue-grey lighten-4" :to="{ name: 'create-course' }"
          >Add new course</v-btn
        >
      </v-btn-toggle>
    </v-row>
    <v-row justify="center">
      <v-col cols="6">
        <router-view></router-view>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col cols="12">
        <ManagementTableComponent :dataSource="courses" :headers="headers" :title="'Courses'" :type="type" :expandDetails="false"/>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from "vue";
import ManagementTableComponent from "@/components/shared-components/ManagementTableComponent.vue";
import storeHelper from "@/store/store-helper";
import { CoursesHeader } from "@/components/shared-components/Headers";
import { ManagementDataType } from "@/shared/enums";
import { CourseViewModule } from "@/modules/course";

export default Vue.extend({
  components: {
    ManagementTableComponent
  },
  data(){
    return {
      headers: CoursesHeader,
      type: ManagementDataType.Course
    };
  },
  computed:{
    courses(): CourseViewModule[]{
      return storeHelper.courseStore.courses;
    }
  },
  async created(){
    await storeHelper.courseStore.loadCourses();
    await storeHelper.userStore.loadCurrentUserInfo()
  }
});
</script>