<template>
  <v-card class="pa-4">
    <validation-observer v-slot="{ handleSubmit }">
      <v-form @submit.prevent="handleSubmit(onSubmitAsync)">
        <v-layout column class="pa-2">
          <v-card-title>
            <p>Generate involvement code</p>
            <v-spacer></v-spacer>
            <v-btn icon @click="onClose">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </v-card-title>
          <v-card-text>
            <v-flex>
              <v-subheader>Expiration time in minutes</v-subheader>
              <v-slider
                v-model="time"
                max="60"
                min="1"
                color="black"
                thumb-label
                prepend-icon="mdi-clock-time-four-outline"
              ></v-slider>
            </v-flex>
            <v-flex v-if="generatedCode !== ''">
              <p v-html="generatedCode" class="pa-2"></p>
            </v-flex>
          </v-card-text>
          <v-card-actions>
            <v-layout class="pa-2" justify-center align-content-end row>
              <v-btn class="mr-1 dark_button white--text" @click="onSubmitAsync">
                Generate code
              </v-btn>
            </v-layout>
          </v-card-actions>
        </v-layout>
      </v-form>
    </validation-observer>
  </v-card>
</template>

<script lang="ts">
import InvolvementCodeService from "@/services/involvement-code.service";
import Vue from "vue";

export default Vue.extend({
  name: "GenerateInvolvementCodeDialog",
  props: {
    collectionId: Number,
  },
  data() {
    return {
      time: 0,
      generatedCode: "",
    };
  },
  methods: {
    onClose: function (): void {
      this.$emit("close");
    },
    onSubmitAsync: async function (): Promise<void> {
      const result = await InvolvementCodeService.createInvolvementCodeAsync({
        minutes: this.time,
        collectionId: this.collectionId,
      });

      if (typeof result !== "undefined") {
        this.generatedCode = `<b><h3>${result.code}</h3></b> (available until ${result.expirationDate})`;
      }
    },
  },
});
</script>