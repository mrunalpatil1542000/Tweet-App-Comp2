import { Formik, Form } from "formik";
import { observer } from "mobx-react-lite";
import React, { useEffect, useState } from "react";
import { Button } from "semantic-ui-react";
import MyTextArea from "../../../app/common/form/MyTextArea";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";

const TweetDetailedChatForm = () => {
	const { userStore, tweetStore } = useStore();
	const [state, setState] = useState(true);
	useEffect(() => {
		if (tweetStore.commentsRegistry.size <= 1 && tweetStore.loadComment) {
			tweetStore.loadComments();
		}
	}, [
		tweetStore.commentsRegistry.size,
		tweetStore.loadComments,
		tweetStore.loading,
		tweetStore.loadingInitial,
		tweetStore.loadComment,
	]);

	return (
		<Formik
			onSubmit={(values, { resetForm }) =>
				tweetStore
					.postAComment(values, userStore.user!, tweetStore.selectedTweet?.id!)
					.then(() => resetForm())
			}
			initialValues={{ message: "" }}
		>
			{({ isSubmitting, isValid }) => (
				<Form className="ui form">
					<MyTextArea placeholder={"message"} name={"message"} rows={2} />
					<Button
						loading={isSubmitting}
						disabled={isSubmitting || !isValid}
						content="Add Reply"
						labelPosition="left"
						icon="edit"
						primary
						floated="right"
						type="submit"
						onClick={() => setState((prev) => !prev)}
					/>
				</Form>
			)}
		</Formik>
	);
};

export default observer(TweetDetailedChatForm);
