import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Segment, Header, Comment, Form, Button } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";
import TweetDetailedChatForm from "./TweetDetailedChatForm";

export default observer(function TweetDetailedChatList() {
	const { tweetStore } = useStore();
	const { loadCurrentComments } = tweetStore;

	return (
		<>
			<Segment
				textAlign="center"
				attached="top"
				inverted
				color="teal"
				style={{ border: "none" }}
			>
				<Header>{loadCurrentComments().length} comments</Header>
			</Segment>
			<Segment attached clearing>
				<Comment.Group>
					{loadCurrentComments().map((x) => {
						return (
							<Comment key={x.id}>
								<Comment.Avatar src="/assets/user.png" />
								<Comment.Content>
									<Comment.Author as="a">
										{x.user.firstName!} {x.user.lastName!}
									</Comment.Author>
									<Comment.Metadata>
										<div>{x.datePosted!}</div>
									</Comment.Metadata>
									<Comment.Text>{x.message!}</Comment.Text>
									<Comment.Actions>
										<Comment.Action>Reply</Comment.Action>
									</Comment.Actions>
								</Comment.Content>
							</Comment>
						);
					})}

					<TweetDetailedChatForm />
				</Comment.Group>
			</Segment>
		</>
	);
});
