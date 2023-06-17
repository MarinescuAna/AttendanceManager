<template>
  <v-card>
    <v-card-title class="ma-3"
      >Change the course <v-spacer></v-spacer>
      <v-btn @click="$emit('close')" icon>
        <v-icon>mdi-close</v-icon>
      </v-btn></v-card-title
    >
    <v-card-text class="pa-4">
      <v-text-field
        label="New course name"
        v-model="name"
        prepend-icon="mdi-pencil"
        required
      />
    </v-card-text>
    <v-select
      :items="specializations"
      label="Specialization"
      v-model="selectedSpecialization"
      prepend-icon="mdi-file"
      item-text="name"
      item-value="id"
      :disabled="specializations.length == 0"
      class="pa-6"
      color="black"
      required
    ></v-select>
    <v-card-actions class="ma-3">
      <v-row justify="center" class="pa-8">
        <v-btn
          color="dark_button white--text"
          @click="onSave"
          :disabled="!isValid"
        >
          Update course
        </v-btn>
      </v-row>
    </v-card-actions>
  </v-card>
</template>
  
  <script lang="ts">
import storeHelper from "@/store/store-helper";
import { CourseViewModule, SpecializationViewModule } from "@/modules/view-modules";
import { UpdateCourseParameters } from "@/modules/commands-parameters";
import Vue from "vue";

export default Vue.extend({
  name: "UpdateCourseDialog",
  props: {
    course: {
      type: Object as () => CourseViewModule,
      required: true,
    },
  },
  data: function () {
    return {
      name: "",
      selectedSpecialization: 0,
    };
  },
  computed: {
    specializations(): SpecializationViewModule[] {
      return storeHelper.userStore.currentUser.specializations;
    },
    isValid: function (): boolean {
      return (
        (this.name != "" &&
        this.name != this.course.name) ||
        this.selectedSpecialization != this.course.specializationId
      );
    },
  },
  created: function (): void {
    if (this.course) {
      const specialization = this.specializations.find(
        (c) => c.id == this.course.specializationId
      );
      if (typeof specialization != "undefined") {
        this.selectedSpecialization = specialization.id;
      }
      this.name = this.course.name;
    }
  },
  methods: {
    /** Save the new name and emit a save event to the parent */
    onSave: async function (): Promise<void> {
      const specialization = this.specializations.find(
        (c) => c.id == this.selectedSpecialization
      )!;
      
      const result = await storeHelper.courseStore.updateCourse(
        {
          courseId: this.course.courseId,
          name: this.name,
          specializationId: this.selectedSpecialization,
        } as UpdateCourseParameters,
        specialization.name
      );

      if (result) {
        this.$emit("save", this.name);
      }
    },
  },
});
</script>