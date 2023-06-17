<template>
  <v-card>
    <v-card-title class="ma-3"
      >Change collection <v-spacer></v-spacer>
      <v-btn @click="$emit('close')" icon>
        <v-icon>mdi-close</v-icon>
      </v-btn></v-card-title
    >
    <v-card-text class="pa-4">
      <v-text-field
        label="New title"
        v-model="collectionTitle"
        prepend-icon="mdi-pencil"
        required
      />
    </v-card-text>
    <v-row class="ma-5">
      <DatePickerComponent
        :date="date"
        @save="getDate"
      />
      <TimePickerComponent
        :time="time"
        @save="getTime"
      />
    </v-row>
    <v-select
      v-model="collectionActivityType"
      :items="courseType"
      label="Activity type"
      prepend-icon="mdi-school"
      class="ma-5"
      required
    ></v-select>
    <v-card-actions class="ma-5">
      <v-row justify="center" class="pa-8">
        <v-btn
          color="dark_button white--text"
          @click="onUpdate"
          :disabled="
            date === collection.activityTime.split(' ')[0] &&
            time === collection.activityTime.split(' ')[1] &&
            collectionTitle === collection.title &&
            collectionActivityType === collection.courseType
          "
        >
          Update collection
        </v-btn>
      </v-row>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
import DatePickerComponent from "@/components/shared-components/DatePickerComponent.vue";
import TimePickerComponent from "@/components/shared-components/TimePickerComponent.vue";
import { UpdateCollectionParameters } from "@/modules/commands-parameters";
import { CollectionDto } from "@/modules/view-modules";
import { Toastification } from "@/plugins/vue-toastification";
import { CourseType } from "@/shared/enums";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
export default Vue.extend({
  name: "UpdateCollectionDialog",
  components: { DatePickerComponent, TimePickerComponent },
  props: {
    collection: {
      type: Object as () => CollectionDto,
      required: true,
    },
  },
  data: function () {
    return {
      date: "",
      time: '',
      courseType: [] as string[],
      collectionTitle: "",
      collectionActivityType: "",
    };
  },
  created: function (): void {
    if (this.collection) {
      this.date = this.collection.activityTime.split(' ')[0];
      this.time = this.collection.activityTime.split(' ')[1];
      this.collectionTitle = this.collection.title;
      this.collectionActivityType = this.collection.courseType;
    }
    if (
      storeHelper.documentStore.report.noLaboratories <
      storeHelper.documentStore.report.maxNoLaboratories
    ) {
      this.courseType.push(CourseType[CourseType.Laboratory]);
    }
    if (
      storeHelper.documentStore.report.noLessons <
      storeHelper.documentStore.report.maxNoLessons
    ) {
      this.courseType.push(CourseType[CourseType.Lecture]);
    }
    if (
      storeHelper.documentStore.report.noSeminaries <
      storeHelper.documentStore.report.maxNoSeminaries
    ) {
      this.courseType.push(CourseType[CourseType.Seminary]);
    }
  },
  methods: {
    onUpdate: async function(): Promise<void>{
        const result = await storeHelper.documentStore.updateCollection({
            activityDateTime: `${this.date} ${this.time}`,
            courseType: this.collectionActivityType,
            collectionId: this.collection.collectionId,
            title: this.collectionTitle
        } as UpdateCollectionParameters);

        if(result){
            Toastification.success(
            "The collection was successfully updated!"
          );
          this.$emit("save",this.collectionTitle,`${this.date} ${this.time}`);
        }
    },
    getDate: function (date): void {
      this.date = date;
    },
    getTime: function (time): void {
      this.time = time;
    },
  },
});
</script>